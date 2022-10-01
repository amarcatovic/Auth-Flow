import { Component, OnInit } from '@angular/core';
import { AuthService } from './shared/services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  public userAuthenticated = false;
  title = 'Weather.Client.Oidc';

  constructor(private _authService: AuthService){
    this._authService.loginChanged
    .subscribe(userAuthenticated => {
      this.userAuthenticated = userAuthenticated;
    })
  }
  
  ngOnInit(): void {
    this._authService.isAuthenticated()
    .then(userAuthenticated => {
      this.userAuthenticated = userAuthenticated;
    })
  }
}
