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
    profilePicture?: Uint8Array;
}

export class CreateUserDto {
    username?: string;
    email?: string;
    password?: string;
    age?: number;
    bio?: string;
    profilePicture?: Uint8Array;
}

export class UserSearchDto {
    userId?: string;
    username?: string;
}

export class StoryDetailsDto {
    storyId?: string;
    storyCreatorId?: string;
    storyCreatorName?: string;
    storyProfilePicture?: Uint8Array;
    imageData?: Uint8Array;
    dateTimePosted?: Date
}

export class ResetPasswordDto {
    email?: string;
    oldPassword?: string;
    newPassword?: string;
}