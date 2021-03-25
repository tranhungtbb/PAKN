import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { PositionObject } from '../../../models/positionObject';
import { PositionService } from 'src/app/services/position.service';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';

@Component({
  selector: 'app-position',
  templateUrl: './position.component.html',
  styleUrls: ['./position.component.css']
})
export class PositionComponent implements OnInit {

  createDataForm: FormGroup;
  model: PositionObject = new PositionObject();
  constructor(

    public formBuilder: FormBuilder,
    private toastr: ToastrService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private positionService: PositionService
  ) { }
  get f() { return this.createDataForm.controls; }
  submitted: boolean = false;
  ngOnInit() {
    this.createDataForm = this.formBuilder.group({
      positionName: [this.model.name, [Validators.required]],
      ma: [this.model.code, [Validators.required]],
      orderNum: [this.model.orderNumber, [Validators.required, Validators.pattern('[0-9]+')]],
      status: [''],
      description: ['']
    });
  }

  onSave(ev: any): void {
    this.submitted = true;
    if (this.createDataForm.invalid)
      return;
    console.log(this.model);
    this.positionService.CreatePosition({ model: this.model }).subscribe(res => {
      if (res.status == 1) {
        if (this.model.id > 0) {
          this.toastr.success("Cập nhật thành công.");
        }
        else {
          this.toastr.success("Thêm mới thành công.");
        }
      }
      else {
        this.toastr.error(res.message);
      }
    })
  }
}
