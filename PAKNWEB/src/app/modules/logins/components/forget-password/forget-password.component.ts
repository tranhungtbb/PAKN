import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from '../../../../services/authentication.service';
import { ForgetPasswordUserObject } from '../../../../models/forgetPasswordUserObject';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-forget-password',
  templateUrl: './forget-password.component.html',
  styleUrls: ['./forget-password.component.css']
})
export class ForgetPasswordComponent implements OnInit {
  submitted: boolean = false;
  user: ForgetPasswordUserObject = {
    Email: '',
  };
  forgetPasswordForm: FormGroup;
  constructor(private authenService: AuthenticationService,
    private _avRoute: ActivatedRoute,
    private _router: Router,
    private toastr: ToastrService,
    private formBuilder: FormBuilder
  ) { }

  ngOnInit() {
    this.forgetPasswordForm = this.formBuilder.group({
      'email': new FormControl(this.user.Email, [
        Validators.required,
      ])
    });
  }

  forgetPassword(): void {
    this.submitted = true;
    if (this.forgetPasswordForm.invalid)
      return;

    this.authenService.forgetpassword(this.user).subscribe((data) => {
      if (data.status === 1) {
        this._router.navigate(['/login']);
      } else if (data.status !== 2) {
        this.toastr.error(data.message);
      }
    }, error => {
      console.error(error);
    });
  }
  login(): void {
    this._router.navigate(['/login']);
  }
  //get email() { return this.forgetPasswordForm.get('email'); }
  get f() { return this.forgetPasswordForm.controls; }
}
