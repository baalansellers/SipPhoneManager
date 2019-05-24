import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PhoneConfigService } from '../core/phone-config.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-phone-config-form',
  templateUrl: './phone-config-form.component.html',
  styleUrls: ['./phone-config-form.component.css']
})
export class PhoneConfigFormComponent implements OnInit {
  isExistingFile: boolean;
  isValidMacAddress: boolean;

  macAddress: string;
  userName: string;
  userExtension: string;
  userPassword: string;

  constructor(
    private _phoneConfigService: PhoneConfigService,
    private _route: ActivatedRoute,
    private _router: Router,
  ) { }

  ngOnInit() {
    const name = this._route.snapshot.paramMap.get('name');

    if( name && name !== "" ) {
      this.macAddress = name;
      this.validateMac(name);
    }

    this._route.data.subscribe((data: {isExistingFile: boolean}) => {
      this.isExistingFile = data.isExistingFile;
      this.isValidMacAddress = !this.isExistingFile;
    });
  }

  onSubmit(f: NgForm) {
    if( this.isExistingFile ) {
      this._phoneConfigService.updateConfiguration(f.value.phoneMacAddress, f.value.name, f.value.extension, f.value.password).subscribe(result => {
        this._router.navigate(['/phones']);
      }, err => {
        console.log("update failed");
      });
    } else {
      this._phoneConfigService.createConfiguration(f.value.phoneMacAddress, f.value.name, f.value.extension, f.value.password).subscribe(result => {
        this._router.navigate(['/phones']);
      }, err => {
        console.log("create failed");
      });
    }
  }

  onBlur(f: NgForm) {
    if( this.isExistingFile ) {
      this.validateMac(f.value.phoneMacAddress);
    }
  }

  onKey(event: any, f: NgForm) {
    if( this.isExistingFile && event.key == "Enter" ) {
      this.validateMac(f.value.phoneMacAddress);
    }
  }

  private validateMac(macAddress: string) {
    this._phoneConfigService.getConfigurationModel(`SEP${macAddress}.cnf.xml`).subscribe(result => {
      this.isValidMacAddress = true;

      this.macAddress = macAddress;
      this.userName = result.name;
      this.userExtension = result.authName;
      this.userPassword = result.authPassword;
    }, err => {
      console.log("bad Mac");
    });
  }
}
