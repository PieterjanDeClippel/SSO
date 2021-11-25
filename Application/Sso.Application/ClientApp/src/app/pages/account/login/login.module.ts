import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LoginRoutingModule } from './login-routing.module';
import { LoginComponent } from './login.component';
import { SsoLoginModule } from '../../../components/sso-login/sso-login.module';


@NgModule({
  declarations: [
    LoginComponent
  ],
  imports: [
    CommonModule,
    SsoLoginModule,
    LoginRoutingModule
  ]
})
export class LoginModule { }
