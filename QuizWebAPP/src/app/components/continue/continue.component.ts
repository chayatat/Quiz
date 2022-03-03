import { Component, OnInit, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { QuizService } from 'src/app/services/quiz.service';
import { SharedService } from 'src/app/services/shared.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-continue',
  templateUrl: './continue.component.html',
  styleUrls: ['./continue.component.css']
})
export class ContinueComponent implements OnInit {

  userForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private ngZone: NgZone,
    private quizService: QuizService,
    private sharedService: SharedService
  ) { 
    this.userForm = this.formBuilder.group({
      userName: ['', Validators.required]
    })
  }

  ngOnInit(): void {
  }

  onSubmit(): any{
    if(this.userForm.value.userName === ''){
      alert("You have to input name"); 
    }else{
      this.quizService.getContinue(this.userForm.value.userName)
        .subscribe((res: any)=>{
          //console.log("res",res);
          if(res.message){
            alert(res.message);
          }else{
            this.sharedService.setUserShared(res);
            if(res.UserStatus === "N"){ // to summary
              this.ngZone.run( ()=> {this.router.navigateByUrl('/summary')} );
            }else{ // to quiz
              this.ngZone.run( ()=> {this.router.navigateByUrl('/quiz')} );
            }
          }
        }, (err) => {
          console.log(err);
        })
    }
  }
}
