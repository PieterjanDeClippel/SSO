import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { BaseLoginComponent } from '../base-login/base-login.component';

@Component({
  selector: 'app-sso-login',
  templateUrl: './sso-login.component.html',
  styleUrls: ['./sso-login.component.scss']
})
export class SsoLoginComponent extends BaseLoginComponent implements OnInit, OnDestroy {

  constructor(@Inject('EXTERNAL_URL') externalUrl: string) {
    super(externalUrl, 'central');
  }

  ngOnInit() {
  }

  ngOnDestroy() {
    super.dispose();
  }

}
