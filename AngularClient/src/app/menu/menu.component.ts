import { Component, OnInit } from '@angular/core';
import { AuthService } from '../shared/services/auth.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {
  public isUserAuthenticated: boolean = false;
  public isUserAdmin: boolean = false;
  public roles: Array<string>;

  constructor(private _authService: AuthService) { }

  ngOnInit(): void {
    this._authService.loginChanged
      .subscribe(res => {
        this.isUserAuthenticated = res;
      });
    this._authService.adminRoleUpdated
      .subscribe(res => {
        this.isUserAdmin = res;
      });
  }

  public login = () => {
    this._authService.login();
  }

  public logout = () => {
    this._authService.logout();
  }
}
