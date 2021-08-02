import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthenticationComponent } from './authentication.component';
import { SignInComponent } from './sign-in/sign-in.component';
import { SignUpComponent } from './sign-up/sign-up.component';
import { AuthenticationRoutesModule } from './authentication.routes';
import { RouterModule } from '@angular/router';

@NgModule({
  imports: [
    CommonModule,
    AuthenticationRoutesModule,
    RouterModule
  ],
  declarations: [
    AuthenticationComponent,
    SignInComponent,
    SignUpComponent
  ]
})
export class AuthenticationModule { }
