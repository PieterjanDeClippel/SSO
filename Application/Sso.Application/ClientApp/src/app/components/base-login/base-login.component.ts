import { Input, Output, EventEmitter, Inject, Directive } from '@angular/core';
import { LoginResult } from '../../entities/login-result';

@Directive()
export class BaseLoginComponent {
  protected authWindow: Window | null = null;
  protected listener: any;
  public isOpen: boolean = false;

  @Input() public action: 'add' | 'connect' = 'connect';
  @Output() public LoginSuccessOrFailed: EventEmitter<LoginResult> = new EventEmitter();

  constructor(
    private externalUrl: string,
    private platform: string,
  ) {
    this.listener = this.handleMessage.bind(this);
    if (typeof window !== 'undefined') {
      if (window.addEventListener) {
        window.addEventListener('message', this.listener, false);
      } else {
        (<any>window).attachEvent('onmessage', this.listener);
      }
    }
  }

  protected dispose() {
    if (typeof window !== 'undefined') {
      if (window.removeEventListener) {
        window.removeEventListener('message', this.listener, false);
      } else {
        (<any>window).detachEvent('onmessage', this.listener);
      }
    }
  }

  showPopup() {
    if (typeof window !== 'undefined') {
      this.authWindow = window.open(`${this.externalUrl}/web/Account/${this.action}/${this.platform}`, '_blank', 'width=600,height=400');

      this.isOpen = true;
      var timer = setInterval(() => {
        if (this.authWindow && this.authWindow.closed) {
          this.isOpen = false;
          clearInterval(timer);
        }
      });
    }
  }

  handleMessage(event: Event) {
    const message = event as MessageEvent;
    console.log('received message', message);

    // Only trust messages from the below origin.
    const messageOrigin = message.origin.replace(/^https?\:/, '');
    const externalUrlWithoutScheme = this.externalUrl.replace(/^https?\:/, '');
    if (!externalUrlWithoutScheme.startsWith(messageOrigin)) return;

    // Filter out Augury
    if (message.data.messageSource != null)
      if (message.data.messageSource.indexOf('AUGURY_') > -1) return;
    // Filter out any other trash
    if (message.data == '' || message.data == null) return;

    const result = <LoginResult>JSON.parse(message.data);
    console.log('result', result);
    if (result.provider === this.platform) {
      this.authWindow && this.authWindow.close();
      this.LoginSuccessOrFailed.emit(result);
    }
  }
}
