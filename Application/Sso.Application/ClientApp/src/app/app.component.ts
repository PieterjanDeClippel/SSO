import { Component, ElementRef } from '@angular/core';
import { User } from './entities/user';
import { LoginComponent } from './pages/account/login/login.component';
import { AccountService } from './services/account/account.service';

@Component({
  selector: 'app-root',
  styleUrls: ['./app.component.scss'],
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'app';
  activeUser: User | null = null;

  constructor(private accountService: AccountService) {
    this.accountService.currentUser().subscribe((user) => {
      this.activeUser = user;
    });
  }

  loginCompleted = (user: User) => {
    this.activeUser = user;
  }

  logoutClicked() {
    this.accountService.logout().subscribe(() => {
      this.activeUser = null;
    });
  }

  routingActivated(element: ElementRef) {
    // Login complete
    if (element instanceof LoginComponent) {
      element.loginComplete.subscribe(this.loginCompleted);
    }
  }

  routingDeactivated(element: ElementRef) {
    // Login complete
    if (element instanceof LoginComponent) {
      element.loginComplete.unsubscribe();
    }
  }
}
