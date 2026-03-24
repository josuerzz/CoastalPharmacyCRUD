import { Component, Injector, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from "@angular/forms";
import { UserCreateDto } from '../../../models/coastalpharmacy.models';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { AuthService } from '../../../core/services/auth.service';
import { CommonModule } from '@angular/common';
import { RouterLink, Router } from "@angular/router";
import { HttpErrorResponse } from '@angular/common/http';
import Swal from 'sweetalert2';
import { Footer } from '../../components/footer/footer';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, 
            CommonModule, 
            RouterLink,
            FontAwesomeModule,
            Footer],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register
{
  faEye = faEye;
  faEyeSlash = faEyeSlash;
  showPassword: boolean = false;

  private authService = inject(AuthService);
  private fb = inject(FormBuilder);
  private router = inject(Router);

  registerForm = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    name: ['', [Validators.required]],
    surname: ['', [Validators.required]],
    password: ['', [Validators.required, Validators.minLength(8)]],
  });

  toggleShow()
  {
    this.showPassword = !this.showPassword;
  }

  onRegister()
  {
    if(this.registerForm.valid)
    {
      const payload: UserCreateDto = this.registerForm.getRawValue(); 

      this.authService.registerUser(payload).subscribe({
        next: () => 
        {
          this.resetForm()
          Swal.fire('Success', 'User created succesfully. Sign in now', 'success');
        },
        error: (err: HttpErrorResponse) =>
        {
          Swal.fire('Error', `Something was wrong. ${err.message} `, 'error');
        }
      })
    }
    else
    {
      this.registerForm.markAllAsTouched();
    }
  }

  resetForm()
  {
    this.registerForm.reset();
  }

  get passwordValue(): string
  {
    return this.registerForm.get('password')?.value ?? '';
  }
}