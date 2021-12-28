import { NgModule } from '@angular/core'
import { BrowserModule } from '@angular/platform-browser'
import { FormsModule } from '@angular/forms'
import { CommonModule } from '@angular/common'
import { CommentComponent } from './comment-recommendation.component'
import { CommentReplyComponent } from '../comment-reply/comment-reply.component'
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [CommentComponent, CommentReplyComponent
  ],
  imports: [CommonModule, FormsModule, RouterModule],
  exports: [CommentComponent, CommentReplyComponent]
})
export class CommentModule { }
