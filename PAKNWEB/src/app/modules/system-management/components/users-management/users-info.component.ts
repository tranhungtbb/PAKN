import { Component, OnInit } from '@angular/core';
import { DepartmentTree } from '../../../../models/departmentTree';
import { MenuPassingObject } from '../orgnization/menuPassingObject';
import { Router } from '@angular/router';

@Component({
  selector: 'app-users-management',
  template: `
            <router-outlet>
                </router-outlet>
            `
})
export class UsersInfoComponent implements OnInit {
  ngOnInit(): void { }
}
