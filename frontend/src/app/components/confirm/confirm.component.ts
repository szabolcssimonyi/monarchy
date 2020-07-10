import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Message } from 'src/app/interfaces/message';

@Component({
  selector: 'app-confirm',
  templateUrl: './confirm.component.html',
  styleUrls: ['./confirm.component.scss']
})
export class ConfirmComponent implements OnInit {
  public showCancel = true;

  constructor(
    public dialogRef: MatDialogRef<ConfirmComponent>,
    @Inject(MAT_DIALOG_DATA) public message: Message) { }

  ngOnInit(): void {
    if (typeof this.message.showCancel !== undefined) {
      this.showCancel = this.message.showCancel;
    }
  }

}
