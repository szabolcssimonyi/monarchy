import { Directive, Input, HostBinding, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';

@Directive({
  selector: '[appHasPermission]'
})
export class HasPermissionDirective implements OnInit {

  @Input('appHasPermission') permissions = [] as string[];
  @HostBinding('style.display') displayType;

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    const hasPermission =
      !Boolean(this.permissions)            // if not initialized
      || !this.permissions.some(p => true)  // if permissions is an empty array
      || this.permissions.some(p => {       // if user permissions array contains at least on permission
        const module = p.split('.')[0];     // from the permissions array of the directive
        const action = p.split('.')[1];
        return this.userService.hasPermission(module, action);
      });
    if (!hasPermission) {
      this.displayType = 'none';
    }
  }


}
