import { Component, Input, Output, EventEmitter } from '@angular/core';
import { User } from '../../entities/user';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {

  isExpanded = false;
  @Input() public activeUser!: User | null;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  @Output() public logoutClicked = new EventEmitter();
  logout() {
    this.logoutClicked.emit();
  }
}
