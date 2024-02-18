import { NgModule, importProvidersFrom } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserHomeComponent } from './components/user/user-home/user-home.component';
import { UserLayoutComponent } from './components/user/user-layout/user-layout.component';
export const routes: Routes = [
  {
    path: '',
    component: UserLayoutComponent,
    children: [{ path: '', component: UserHomeComponent }],
  },
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
