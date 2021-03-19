import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PageForbiddenComponent } from './page-forbidden/page-forbidden.component';

const routes: Routes = [{
  path: 'forbidden',
  component: PageForbiddenComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ForbidenPageRoutingModule { }
