import { Component, OnInit, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { QuizService } from 'src/app/services/quiz.service';
import { SharedService } from 'src/app/services/shared.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  UserGroups: any = [];
  userForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private ngZone: NgZone,
    private quizService: QuizService,
    private sharedService: SharedService
  ) { 
    this.userForm = this.formBuilder.group({
      userGroupId: ['', Validators.required],
      userName: ['', Validators.required]
    })
  }

  ngOnInit(): void {
    this.getService();
  }

  async getService(){
    this.UserGroups = await this.quizService.getUserGroup().toPromise();
  }

  onSubmit(): any{
    if(this.userForm.value.userGroupId === ''){
      alert("Please select user group"); 
    }else if(this.userForm.value.userName === ''){
      alert("You have to input name"); 
    }else{
      this.quizService.addRegister(this.userForm.value)
        .subscribe((res: any)=>{
          //console.log("res",res);
          if(res.message){
            alert(res.message);
          }else{
            this.sharedService.setUserShared(res);
            this.ngZone.run( ()=> {this.router.navigateByUrl('/quiz')} );
          }
        }, (err) => {
          console.log(err);
        })
    }
  }
}
