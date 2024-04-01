import { ResetPasswordComponent } from './components/auth/reset-password/reset-password.component';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { Router, RouterModule } from '@angular/router';
import { DatePipe, NgOptimizedImage } from '@angular/common';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CodeInputModule } from 'angular-code-input';
import { JwtModule } from '@auth0/angular-jwt';
import { Environment } from './environment/environment';
import { TokenInterceptor } from './interceptors/token.interceptor';
import { EditorModule, TINYMCE_SCRIPT_SRC } from '@tinymce/tinymce-angular';
import { NgSelectModule } from '@ng-select/ng-select';
import { ForgotPasswordComponent } from './components/auth/forgot-password/forgot-password.component';
import { NgxDropzoneModule } from 'ngx-dropzone';

import { register } from 'swiper/element/bundle';
import { AdminLayoutComponent } from './components/admin/admin-layout/admin-layout.component';
import { AdminFooterComponent } from './components/admin/admin-footer/admin-footer.component';
import { AdminHeaderComponent } from './components/admin/admin-header/admin-header.component';
import { AdminHomeComponent } from './components/admin/admin-home/admin-home.component';
import { AdminMenuComponent } from './components/admin/admin-menu/admin-menu.component';
import { AuthComponent } from './components/admin/auth/auth.component';
import { CategoryComponent } from './components/admin/category/category.component';
import { CustomerComponent } from './components/admin/customer/customer.component';
import { EmployeeComponent } from './components/admin/employee/employee.component';
import { ProductComponent } from './components/admin/product/product.component';
import { InsertAdminProductComponent } from './components/admin/product/insert/insert-admin-product.component';
import { PromotionComponent } from './components/admin/promotion/promotion.component';
import { SupplierAdminComponent } from './components/admin/supplier/supplier.admin/supplier.admin.component';
import { TrademarkComponent } from './components/admin/trademark/trademark.component';
import { SignInComponent } from './components/auth/sign-in/sign-in.component';
import { SignOutComponent } from './components/auth/sign-out/sign-out.component';
import { SignUpComponent } from './components/auth/sign-up/sign-up.component';
import { VerificationCodeComponent } from './components/auth/verification-code/verification-code.component';
import { HomeComponent } from './components/client/home/home.component';
import { DetailProductComponent } from './components/client/detail-product/detail-product.component';
import { CheckoutComponent } from './components/client/checkout/checkout.component';
import { ClientLayoutComponent } from './components/client/client-layout/client-layout.component';
import { ClientFooterComponent } from './components/client/client-footer/client-footer.component';
import { ClientHeaderComponent } from './components/client/client-header/client-header.component';
import { CartComponent } from './components/client/cart/cart.component';
import { ProductsFilterComponent } from './components/client/products-filter/products-filter.component';
import { ShippingInfoComponent } from './components/client/shipping-info/shipping-info.component';
import { AppRoutingModule, routes } from './app.routes';
import { AppComponent } from './app.component';
import { InventoryReceiptsComponent } from './components/admin/inventory.receipts/inventory.receipts.component';
import { InsertInventoryReceiptsComponent } from './components/admin/inventory.receipts/insert.inventory.receipts/insert.inventory.receipts.component';
import { CurrencyFormatPipe } from './pipe/currency-format.pipe';
import { AccessForbiddenComponent } from './components/auth/errors/access-forbidden/access-forbidden.component';
import { InsertPromotionComponent } from './components/admin/promotion/Insert/insert-promotion/insert-promotion.component';

register();

@NgModule({
  declarations: [
    AppComponent,
    AdminFooterComponent,
    AdminHeaderComponent,
    AdminHomeComponent,
    AdminLayoutComponent,
    AdminMenuComponent,
    AuthComponent,
    CategoryComponent,
    CustomerComponent,
    EmployeeComponent,
    ProductComponent,
    InsertAdminProductComponent,
    PromotionComponent,
    SupplierAdminComponent,
    TrademarkComponent,
    ForgotPasswordComponent,
    SignInComponent,
    SignOutComponent,
    SignUpComponent,
    VerificationCodeComponent,
    HomeComponent,
    DetailProductComponent,
    CheckoutComponent,
    ClientLayoutComponent,
    ClientFooterComponent,
    ClientHeaderComponent,
    CartComponent,
    ProductsFilterComponent,
    ShippingInfoComponent,
    InventoryReceiptsComponent,
    InsertInventoryReceiptsComponent,
    CurrencyFormatPipe,
    AccessForbiddenComponent,
    ResetPasswordComponent,
    InsertPromotionComponent,
  ],
  imports: [
    RouterModule.forRoot(routes),
    BrowserModule,
    AppRoutingModule,
    NgOptimizedImage,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    ToastrModule.forRoot({
      timeOut: 5000,
      positionClass: 'toast-top-right',
      preventDuplicates: true,
      tapToDismiss: true,
      extendedTimeOut: 1000,
    }),
    BrowserAnimationsModule,
    CodeInputModule.forRoot({
      codeLength: 6,
      isCharsCode: false,
    }),
    JwtModule.forRoot({
      config: {
        tokenGetter: () => {
          return localStorage.getItem('token');
        },
        allowedDomains: [`${Environment.apiBaseRoot}`],
        disallowedRoutes: [`${Environment.apiBaseUrl}/login`],
      },
    }),
    EditorModule,
    NgSelectModule,
    NgxDropzoneModule,
  ],
  exports: [RouterModule],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true,
    },
    // {
    //   provide: TINYMCE_SCRIPT_SRC,
    //   useValue: 'tinymce/tinymce.min.js',
    // },
    DatePipe,
  ],
  bootstrap: [AppComponent], // Remove AppComponent from the bootstrap array
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule {}
