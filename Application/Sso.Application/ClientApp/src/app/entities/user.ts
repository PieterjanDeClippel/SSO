import { Guid } from 'guid-typescript';

export interface User {
  id: Guid;
  userName: string;
  email: string;
  phone: string;
  dateOfBirth: Date;

  isTwoFactorEnabled: boolean;
  bypass2faForExternalLogin: boolean;
}
