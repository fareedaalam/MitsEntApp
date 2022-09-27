export interface User{
    username:string;
    token :string;
    photoUrl:string;
    knownAs:string;
    gender:string;
    isActive:boolean;
    roles:string[];
}

export interface OTP{
    username:string;
    phonenumber:string;
    otp:string;
}