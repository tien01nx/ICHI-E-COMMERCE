import { OrderComponent } from './components/admin/order/order.component';
import { CustomerComponent } from './components/admin/customer/customer.component';
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
import { AdminHomeComponent } from './components/admin/admin-home/admin-home.component';
import { NgModule } from '@angular/core';
import { ProductComponent } from './components/admin/product/product.component';
import { InsertAdminProductComponent } from './components/admin/product/insert/insert-admin-product.component';
import { CategoryComponent } from './components/admin/category/category.component';
import { RouterModule, Routes } from '@angular/router';
import { SupplierAdminComponent } from './components/admin/supplier/supplier.admin/supplier.admin.component';
import { EmployeeComponent } from './components/admin/employee/employee.component';
import { AuthComponent } from './components/admin/auth/auth.component';
import { TrademarkComponent } from './components/admin/trademark/trademark.component';
import { PromotionComponent } from './components/admin/promotion/promotion.component';
import { PromotiondemoComponent } from './components/admin/promotiondemo/promotiondemo.component';
import { AdminGuard } from './guard/admin.guard';
import { InventoryReceiptsComponent } from './components/admin/inventory.receipts/inventory.receipts.component';
import { InsertInventoryReceiptsComponent } from './components/admin/inventory.receipts/insert.inventory.receipts/insert.inventory.receipts.component';
import { AccessForbiddenComponent } from './components/auth/errors/access-forbidden/access-forbidden.component';
import { ResetPasswordComponent } from './components/auth/reset-password/reset-password.component';
import { UserGuard } from './guard/user.guard';
import { InsertPromotionComponent } from './components/admin/promotion/Insert/insert-promotion/insert-promotion.component';
import { ListOrderComponent } from './components/admin/order/list-order/list-order.component';
import { ProfileComponent } from './components/client/profile/profile.component';
import { OrderTrackingComponent } from './components/client/order-tracking/order-tracking.component';
import { InvoiceComponent } from './components/client/invoice/invoice.component';
import { OrderDetailComponent } from './components/admin/order-detail/order-detail.component';
import { BuyNowComponent } from './components/client/checkout/buy-now/buy-now.component';
import { ProductDetailComponent } from './components/client/product/product-detail/product-detail.component';

export const routes: Routes = [
  {
    path: 'admin',
    component: AdminLayoutComponent,
    children: [
      { path: '', component: AdminHomeComponent },
      { path: 'products', component: ProductComponent },
      { path: 'product/insert', component: InsertAdminProductComponent },

      { path: 'product/insert/:id', component: InsertAdminProductComponent },
      { path: 'categories', component: CategoryComponent },
      { path: 'supplier', component: SupplierAdminComponent },
      { path: 'customer', component: CustomerComponent },
      { path: 'employee', component: EmployeeComponent },
      { path: 'account_manager', component: AuthComponent },
      { path: 'auth', component: AuthComponent },
      { path: 'trademark', component: TrademarkComponent },
      { path: 'promotion', component: PromotionComponent },
      { path: 'promotion_insert', component: InsertPromotionComponent },
      { path: 'promotion_insert/:id', component: InsertPromotionComponent },
      { path: 'promotiondemo', component: PromotiondemoComponent },
      { path: 'inventory_receipts', component: InventoryReceiptsComponent },
      {
        path: 'insert_inventory_receipts',
        component: InsertInventoryReceiptsComponent,
      },
      { path: 'order', component: OrderComponent },
      { path: 'order_detail/:id', component: OrderDetailComponent },
      { path: 'list_order', component: ListOrderComponent },
      {
        path: 'insert_inventory_receipts/:id',
        component: InsertInventoryReceiptsComponent,
      },
    ],
    canActivate: [AdminGuard],
  },
  { path: '', component: ClientLayoutComponent },
  // { path: 'product_detail/:id', component: DetailProductComponent },
  { path: 'product_detail/:id', component: ProductDetailComponent },
  { path: 'product_filter/:categoryName', component: ProductsFilterComponent },
  { path: 'cart', component: CartComponent, canActivate: [UserGuard] },
  { path: 'checkout', component: CheckoutComponent, canActivate: [UserGuard] },
  { path: 'checkout-buy-now', component: BuyNowComponent },
  {
    path: 'checkout/:id',
    component: CheckoutComponent,
    canActivate: [UserGuard],
  },
  {
    path: 'shipping_info',
    component: ShippingInfoComponent,
    canActivate: [UserGuard],
  },
  { path: 'login', component: SignInComponent },
  { path: 'register', component: SignUpComponent },
  { path: 'forgot_password', component: ForgotPasswordComponent },
  { path: 'verify_email', component: VerificationCodeComponent },
  { path: 'access-denied', component: AccessForbiddenComponent },
  { path: 'invoice', component: InvoiceComponent },
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [UserGuard],
  },
  {
    path: 'order_tracking/:id',
    component: OrderTrackingComponent,
    canActivate: [UserGuard],
  },
  {
    path: 'reset-password',
    component: ResetPasswordComponent,
    canActivate: [UserGuard],
  },
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
