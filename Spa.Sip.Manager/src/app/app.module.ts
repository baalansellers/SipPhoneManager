import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { HttpClientModule } from '@angular/common/http';
import { HomeComponent } from './home/home.component';
import { ConfigurationsComponent } from './configurations/configurations.component';
import { PhoneListComponent } from './phone-list/phone-list.component';
import { ViewConfigContentComponent } from './view-config-content/view-config-content.component';
import { PhoneConfigFormComponent } from './phone-config-form/phone-config-form.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ConfigurationsComponent,
    PhoneListComponent,
    ViewConfigContentComponent,
    PhoneConfigFormComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CoreModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
