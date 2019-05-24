import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ConfigurationsComponent } from './configurations/configurations.component';
import { PhoneListComponent } from './phone-list/phone-list.component';
import { ViewConfigContentComponent } from './view-config-content/view-config-content.component';
import { PhoneConfigFormComponent } from './phone-config-form/phone-config-form.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'home', component: HomeComponent },
  { path: 'test', component: ConfigurationsComponent },
  { path: 'phones', component: PhoneListComponent },
  { path: 'configuration/:name', component: ViewConfigContentComponent},
  { path: 'provision', component: PhoneConfigFormComponent, data: { isExistingFile: false } },
  { path: 'reprovision', component: PhoneConfigFormComponent, data: { isExistingFile: true } },
  { path: 'reprovision/:name', component: PhoneConfigFormComponent, data: { isExistingFile: true } },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
