import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TreeModule } from 'primeng/tree';
import { DepartmentTreeComponent } from './department-tree.component';


@NgModule({

  imports: [
    CommonModule, 
    TreeModule
  ], declarations: [DepartmentTreeComponent]
  ,exports: [
    DepartmentTreeComponent
  ]
})
export class DePartmentTreeModule { }
