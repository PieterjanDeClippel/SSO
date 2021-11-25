import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SsoLoginComponent } from './sso-login.component';



@NgModule({
  declarations: [
    SsoLoginComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    SsoLoginComponent
  ]
})
export class SsoLoginModule { }
