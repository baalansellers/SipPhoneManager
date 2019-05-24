import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewConfigContentComponent } from './view-config-content.component';

describe('ViewConfigContentComponent', () => {
  let component: ViewConfigContentComponent;
  let fixture: ComponentFixture<ViewConfigContentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewConfigContentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewConfigContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
