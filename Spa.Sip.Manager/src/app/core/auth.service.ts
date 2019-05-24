import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { UserManager, User, WebStorageStateStore } from 'oidc-client';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _userManager: UserManager;
  private _user: User;

  constructor(private httpClient: HttpClient) {
    //var client_root = 'https://localhost:4200';
    var client_root = 'https://baalansellers.github.io/SIP-MGR-SPA';
    var config = {
      authority: 'https://id-sip-mgr.azurewebsites.net',
      client_id: 'sip.mgr',
      redirect_uri: `${client_root}/assets/login-callback.html`,
      scope: 'openid profile api.sip.manager',
      response_type: 'code',
      post_logout_redirect_uri: `${client_root}/?postLogout=true`,
      silent_redirect_uri: `${client_root}/assets/renew-callback.html`,
      userStore: new WebStorageStateStore({store: window.localStorage}),
      automaticSilentRenew: true,
    };

    this._userManager = new UserManager(config);

    this._userManager.getUser().then(user => {
      if ( user && !user.expired ) {
        this._user = user;
      }
    });

    this._userManager.events.addUserLoaded(() => {
      this._userManager.getUser().then(user => {
        this._user = user;
      });
    });
  }

  login(): Promise<any> {
    return this._userManager.signinRedirect();
  }

  logout(): Promise<any> {
    return this._userManager.signoutRedirect();
  }

  isLoggedIn(): boolean {
    return this._user && this._user.access_token && !this._user.expired;
  }

  getAccessToken(): string {
    return this._user ? this._user.access_token : '';
  }

  signoutRedirectCallback(): Promise<any> {
    return this._userManager.signoutPopupCallback();
  }
}
