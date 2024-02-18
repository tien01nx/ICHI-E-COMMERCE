import { RouterModule, Routes } from '@angular/router';
import { ClientLayoutComponent } from './components/client/client-layout/client-layout.component';
import { DetailProductComponent } from './components/client/detail-product/detail-product.component';
import { ProductsFilterComponent } from './components/client/products-filter/products-filter.component';
import { CartComponent } from './components/client/cart/cart.component';
import { CheckoutComponent } from './components/client/checkout/checkout.component';
import { ShippingInfoComponent } from './components/client/shipping-info/shipping-info.component';
import { SignInComponent } from './components/auth/sign-in/sign-in.component';
import { SignUpComponent } from './components/auth/sign-up/sign-up.component';
import { ForgotPasswordComponent } from './components/auth/forgot-password/forgot-password.component';
import { VerificationCodeComponent } from './components/auth/verification-code/verification-code.component';
import { AdminLayoutComponent } from './components/admin/admin-layout/admin-layout.component';
import { AdminFooterComponent } from './components/admin/admin-footer/admin-footer.component';
import { AdminHeaderComponent } from './components/admin/admin-header/admin-header.component';
import { AdminHomeComponent } from './components/admin/admin-home/admin-home.component';
import { NgModule } from '@angular/core';

export const routes: Routes = [
  {
    path: 'admin',
    component: AdminLayoutComponent,

    children: [{ path: '', component: AdminHomeComponent }],
  },
  { path: '', component: ClientLayoutComponent },
  { path: 'product_detail', component: DetailProductComponent },
  { path: 'product_filter', component: ProductsFilterComponent },
  { path: 'cart', component: CartComponent },
  { path: 'checkout', component: CheckoutComponent },
  { path: 'shipping_info', component: ShippingInfoComponent },
  { path: 'login', component: SignInComponent },
  { path: 'register', component: SignUpComponent },
  { path: 'forgot_password', component: ForgotPasswordComponent },
  { path: 'verify_email', component: VerificationCodeComponent },
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
