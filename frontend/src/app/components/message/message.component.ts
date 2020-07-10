import { Component, OnInit, Inject } from '@angular/core';
import { MAT_SNACK_BAR_DATA } from '@angular/material/snack-bar';
import { Message } from 'src/app/interfaces/message';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.scss']
})
export class MessageComponent implements OnInit {

  constructor(@Inject(MAT_SNACK_BAR_DATA) public message: Message) { }

  ngOnInit(): void {
  }

}
