import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PhoneConfigFormComponent } from './phone-config-form.component';

describe('PhoneConfigFormComponent', () => {
  let component: PhoneConfigFormComponent;
  let fixture: ComponentFixture<PhoneConfigFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PhoneConfigFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PhoneConfigFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
