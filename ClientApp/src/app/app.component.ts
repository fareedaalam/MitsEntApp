import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent  implements OnInit{
  title = 'MitsEntertainment';
  users:any;

  constructor(private http:HttpClient){

  }
  ngOnInit(): void {
    this.getUsers();
    
   
}

getUsers(){
  this.http.get('https://localhost:5001/api/users').subscribe({
    next:responce => this.users = responce,
    error:error=> console.log(error)
  })     
}

}
