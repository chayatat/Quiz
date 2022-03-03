import { Component, OnInit, NgZone } from '@angular/core';
import { Router, ActivatedRoute  } from '@angular/router';
import { QuizService } from 'src/app/services/quiz.service';
import { SharedService } from 'src/app/services/shared.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-quiz',
  templateUrl: './quiz.component.html',
  styleUrls: ['./quiz.component.css']
})
export class QuizComponent implements OnInit {

  Questions: any = [];
  UserScores: any = [];
  userScoresForm: FormGroup;

  getUserGroupId: any;
  user: any;
  msg: any;
  userScoresArr: any[] = [];
  
  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private ngZone: NgZone,
    private quizService: QuizService,
    private activatedRoute: ActivatedRoute,
    private sharedService: SharedService
  ) { 
    this.user = this.sharedService.getUserShared();
    if (typeof(this.user) == "undefined"){
      this.ngZone.run( ()=> {this.router.navigateByUrl('/continue')} );
    }
    this.userScoresForm = this.formBuilder.group({
      ques : this.formBuilder.array([])
    })
  }
  
  ngOnInit(): void {
    // console.log("ngOnInit");
    
    this.getServiceQuiz();
  }

  async getServiceQuiz(){
    // console.log("getServiceQuiz Start");

    this.Questions = await this.quizService.getQuiz(this.user.UsergroupId).toPromise();
    this.UserScores = await this.quizService.getLoad(this.user.UserId).toPromise();
    await this.appenArr();

    // console.log("getServiceQuiz End");
  }
  

  appenArr(){
    // console.log("appenArr");
    // console.log("Questions", this.Questions);
    // console.log("UserScores", this.UserScores);
    // console.log("userScoresArr", this.userScoresArr);
    
    this.userScoresArr = [];
    this.Questions.forEach( (q:any, index:any) => {
      // console.log('q', index,q);
      q.UserScoreId = 0;
      q.ChoiceIdCheck = 0;
      this.Questions[index] = q;

      this.UserScores.forEach( (s:any) => {
        if(q.QuestionId == s.QuestionId){
          q.UserScoreId = s.UserScoreId;
          q.ChoiceIdCheck = s.ChoiceId;
          this.Questions[index] = q;

          this.userScoresArr.push(
            {
              userScoreId: s.UserScoreId,
              userId: this.user.UserId,
              userName: this.user.UserName,
              userGroupId: this.user.UsergroupId,
              userGroupName: "",
              questionId: s.QuestionId,
              choiceId: s.ChoiceId,
              choiceScore: s.ChoiceScore
            }
          );
        }
      })
    })

    // console.log("userScoresArr", this.userScoresArr);
    // console.log("appenArr end");
  }

  changeHandler(evnet:any){
    let id = evnet.target.id.split("_")[2];
    let q = evnet.target.name.split("_")[1];
    let c = evnet.target.id.split("_")[1];
    let v = evnet.target.value;
    
    let temp = {
      userScoreId: id,
      userId: this.user.UserId,
      userName: this.user.UserName,
      userGroupId: this.user.UsergroupId,
      userGroupName: "",
      questionId: q,
      choiceId: c,
      choiceScore: v
    }
    

    let index = this.userScoresArr.findIndex( c => c.questionId == q );
    // console.log("index", index);
    if(index >= 0){
      this.userScoresArr[index].choiceId = c;
      this.userScoresArr[index].choiceScore = v;
    }else{
      this.userScoresArr.push(temp);
    }
    // console.log("changeHandler", this.userScoresArr);
  }

  async onSave(){
    // console.log("onSave");
    if(this.userScoresArr.length === 0){
      alert("Please select the desired answer.");
    }else{
      
      this.msg = await this.quizService.saveQuiz(this.userScoresArr).toPromise();
      alert(this.msg.message);
      
      await this.getServiceQuiz();
    }
  }

  onSubmit(){
    // console.log("onSubmit");
    if(this.userScoresArr.length < this.Questions.length){
      alert("Please select complete answers to all questions.");
    }else{
      this.sharedService.setUserShared(this.user);
      this.quizService.submitQuiz(this.userScoresArr)
        .subscribe((res: any)=>{
          alert(res.message);
          this.ngZone.run( ()=> {this.router.navigateByUrl('/summary')} );
        }, (err) => {
          console.log(err);
        })
    }
  }

}
