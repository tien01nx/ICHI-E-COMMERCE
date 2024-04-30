import { Injectable } from '@angular/core';
import { TokenService } from '../service/token.service';
import { ToastrService } from 'ngx-toastr';
import {
  ActivatedRouteSnapshot,
  CanActivateFn,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AdminGuard {
  constructor(
    private tokenService: TokenService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  canActivate: CanActivateFn = (
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean | UrlTree => {
    //
    const requiredRole = ['ADMIN', 'EMPLOYEE']; // Quyền truy cập yêu cầu
    let roles = this.tokenService.getUserRoles(); // Lấy danh sách các quyền từ AuthService
    // console.log("role:" + roles);
    if (!Array.isArray(roles)) {
      roles = [roles];
    }

    //
    if (
      roles == null ||
      roles.length == 0 ||
      this.tokenService.isTokenExpired()
    ) {
      // nếu token hết hạn, chuyển hướng đến trang đăng nhập
      this.toastr.error('Phiên làm việc hết hạn, vui lòng đăng nhập lại');
      return this.router.createUrlTree(['/login']);
    } else {
      const hasRequiredRole = roles.some((role: any) =>
        requiredRole.includes(role)
      );
      if (hasRequiredRole) {
        return true;
      }
      // Người dùng không có quyền truy cập, chuyển hướng đến trang access-denied
      return this.router.createUrlTree(['/access-denied']);
    }
    // } if (requiredRole.includes(roles)) {
    //   // Nếu người dùng có ít nhất một quyền nằm trong danh sách quyền yêu cầu
    //   return true; // Người dùng có quyền truy cập
    // } else {
    //   // Người dùng không có quyền truy cập, chuyển hướng đến trang access-denied
    //   return this.router.createUrlTree(['/access-denied']);
    // }
  };
}
