import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ResgatarCdbComponent } from './resgatar.component';

describe('ResgatarComponent', () => {
  let component: ResgatarCdbComponent;
  let fixture: ComponentFixture<ResgatarCdbComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ResgatarCdbComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ResgatarCdbComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
