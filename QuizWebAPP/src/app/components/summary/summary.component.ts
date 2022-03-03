import { Component, OnInit, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { QuizService } from 'src/app/services/quiz.service';
import { SharedService } from 'src/app/services/shared.service';


@Component({
  selector: 'app-summary',
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.css']
})
export class SummaryComponent implements OnInit {

  user: any;
  rank: any;
  maxscore: any;

  constructor(
    private router: Router,
    private ngZone: NgZone,
    private quizService: QuizService,
    private sharedService: SharedService) 
    { 
      this.user = this.sharedService.getUserShared();      
      if(this.user){
        this.quizService.getSummary(this.user.UserId)
        .subscribe((res: any)=>{
          this.user = res.user;
          this.rank = res.rank;
          this.maxscore = res.maxscore;
        }, (err) => {
          console.log(err);
        })
        
      }else{
        this.ngZone.run( ()=> {this.router.navigateByUrl('/continue')} );
      }
    }

  ngOnInit(): void {
  }

}
