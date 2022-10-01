import { Injectable } from '@angular/core';
import { UserManager, User, UserManagerSettings } from 'oidc-client';
import { Constants } from '../constants/constants';
import { BehaviorSubject, Subject } from 'rxjs';
import jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _userManager: UserManager;
  private _user: User;
  private _loginChangedSubject = new Subject<boolean>();
  private _userAdminSubject = new Subject<boolean>();
  public loginChanged = this._loginChangedSubject.asObservable();
  public adminRoleUpdated = this._userAdminSubject.asObservable();

  private get idpSettings() : UserManagerSettings {
    return {
      authority: Constants.idpAuthority,
      client_id: Constants.clientId,
      redirect_uri: `${Constants.clientRoot}/signin-callback`,
      scope: "openid profile testAPI",
      response_type: "code",
      post_logout_redirect_uri: `${Constants.clientRoot}/signout-callback`,
      automaticSilentRenew: true,
      silent_redirect_uri: `${Constants.clientRoot}/assets/silent-callback.html`
    }
  }
  constructor() { 
    this._userManager = new UserManager(this.idpSettings);
    this._userManager.events.addAccessTokenExpired(_ => {
      this._loginChangedSubject.next(false);
    });
  }

  public login = (): Promise<void> => {
    return this._userManager.signinRedirect();
  }

  public isAuthenticated = (): Promise<boolean> => {
    return this._userManager.getUser()
      .then(user => {
        if (this._user !== user) {
          this._loginChangedSubject.next(this.checkUser(user));
        }
        this._user = user;
          
        return this.checkUser(user);
      });
  }

  private checkUser = (user : User): boolean => {
    return !!user && !user.expired;
  }

  public finishLogin = (): Promise<User> => {
    return this._userManager.signinRedirectCallback()
    .then(user => {
      this._user = user;
      this._loginChangedSubject.next(this.checkUser(user));
      this.checkIfUserIsAdmin();
      return user;
    })
  }

  public logout = () => {
    this._userManager.signoutRedirect();
  }

  public finishLogout = () => {
    this._user = null;
    this._loginChangedSubject.next(false);
    return this._userManager.signoutRedirectCallback();
  }

  public getAccessToken = (): Promise<string> => {
    return this._userManager.getUser()
      .then(user => {
         return !!user && !user.expired ? user.access_token : null;
    })
  }

  private checkIfUserIsAdmin = (): void => {
    this._userManager.getUser()
      .then(user => {
        const userRoles = this.getUserRoles(user.access_token);
        this._userAdminSubject.next(userRoles.some(r => r === 'Admin'));
      })
  }

  private getUserRoles(token: string): Array<string> | null {
    try {
      const decodedToken: any = jwt_decode(token);
      return decodedToken?.role;
    } catch(Error) {
      return null;
    }
  }
}
