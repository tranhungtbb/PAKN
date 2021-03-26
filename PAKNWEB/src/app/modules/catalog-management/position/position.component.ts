import { Component, OnInit } from '@angular/core'
import { FormGroup, FormBuilder, Validators, FormArray, FormControl } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { PositionObject } from '../../../models/positionObject'
import { PositionService } from 'src/app/services/position.service'
import { ActivatedRoute, ParamMap, Router } from '@angular/router'
import { FieldObject } from 'src/app/models/fieldObject'

import { LOG_ACTION, LOG_OBJECT, MESSAGE_COMMON, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'

declare var $: any
@Component({
  selector: 'app-position',
  templateUrl: './position.component.html',
  styleUrls: ['./position.component.css'],
})
export class PositionComponent implements OnInit {
  createDataForm: FormGroup
  listData = new Array<FieldObject>()
  listStatus: any = [
    { value: true, text: 'Hiệu lực' },
    { value: false, text: 'Hết hiệu lực' },
  ]
  name: string = ''
  description: string = ''
  title: string = 'Thêm mới chức vụ'
  table: any
  isActived: boolean
  pageIndex: number = 1
  pageSize: number = 20
  totalRecords: number = 0
  idDelete: number = 0
  model: PositionObject = new PositionObject()
  constructor(
    public formBuilder: FormBuilder,
    private toastr: ToastrService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private positionService: PositionService
  ) { }
  get f() {
    return this.createDataForm.controls
  }
  submitted: boolean = false
  ngOnInit() {
    this.buildForm()
  }
  onPageChange(event: any) {
    this.pageSize = event.rows
    this.pageIndex = event.first / event.rows + 1
    this.getList()
  }
  buildForm() {
    this.createDataForm = this.formBuilder.group({
      name: [this.model.name, [Validators.required]],
      isActived: [this.model.isActived, [Validators.required]],
      description: [this.model.description],
    })
  }
  dataStateChange() {
    this.pageIndex = 1
    this.getList()
  }
  rebuilForm() {
    console.log(this.model);
    this.createDataForm.reset({
      name: this.model.name,
      isActived: this.model.isActived,
      description: this.model.description,
    })
  }
  getList() {
    this.name = this.name.trim()
    this.description = this.description.trim()

    let request = {
      Name: this.name,
      Description: this.description,
      isActived: this.isActived != null ? this.isActived : '',
      PageIndex: this.pageIndex,
      PageSize: this.pageSize,
    }
    console.log(request)
    this.positionService.positionGetList(request).subscribe((response) => {
      if (response.success == RESPONSE_STATUS.success) {
        if (response.result != null) {
          this.listData = []
          this.listData = response.result.CAPositionGetAllOnPage
          this.totalRecords = response.result.rowNumber
        }
      } else {
        this.toastr.error(response.message)
      }
    }),
      (error) => {
        console.log(error)
        alert(error)
      }
  }
  onSave(): void {
    this.submitted = true
    if (this.createDataForm.invalid) return
    console.log(this.model)
    if (this.model.id == 0 || this.model.id == null) {
      this.positionService.CreatePosition(this.model).subscribe((res) => {
        if (res.result == 1) {
          this.toastr.success('Thêm mới thành công.')

          this.getList()
        } else {
          this.toastr.error(res.message)
        }
        $('#modal-tm-cqdv').modal('hide')
      })
    }
    else {
      this.positionService.UpdatePosition(this.model).subscribe((res) => {
        if (res.result == 1) {
          this.toastr.success('Cập nhật thành công.')

          this.getList()
        } else {
          this.toastr.error(res.message)
        }
        $('#modal-tm-cqdv').modal('hide')
      })
    }
  }
  preDelete(id: number) {
    this.idDelete = id
    $('#modalConfirmDelete').modal('show')
  }
  onDelete(id: number) {
    let request = {
      Id: id,
    }
    this.positionService.positionDelete(request).subscribe((response) => {
      if (response.success == RESPONSE_STATUS.success) {
        this.toastr.success(MESSAGE_COMMON.DELETE_SUCCESS)
        $('#modalConfirmDelete').modal('hide')
        this.getList()
      } else {
        this.toastr.error(response.message)
      }
    }),
      (error) => {
        console.error(error)
      }
  }
  preCreate() {
    this.submitted = false
    this.model = new PositionObject()
    this.rebuilForm()
    this.title = 'Thêm mới chức vụ'
    $('#modal-tm-cqdv').modal('show')
  }
  preUpdate(data) {
    let request = {
      Id: data.id,
      Type: 1,
    }
    this.positionService.positionGetById(request).subscribe((response) => {
      if (response.success == RESPONSE_STATUS.success) {
        this.title = 'Chỉnh sửa chức vụ'
        this.model = response.result.CAPositionGetByID[0]
        console.log(this.model)
        this.rebuilForm()
        $('#modal-tm-cqdv').modal('show')
      } else {
        this.toastr.error(response.message)
      }
    }),
      (error) => {
        console.error(error)
        alert(error)
      }
  }
}
