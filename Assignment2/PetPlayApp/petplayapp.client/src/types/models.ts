export class LoginDto {
    username?: string;
    password?: string;
}

export class UserDetailsDto {
    userId?: string;
    username?: string;
    email?: string;
    age?: number;
    bio?: string;
}

export class CreateUserDto {
    username?: string;
    email?: string;
    password?: string;
    age?: number;
    bio?: string;
}