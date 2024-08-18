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
    interest?: UserInterest;
    status?: UserStatus;
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
    dateTimePosted?: Date;
}

export class ResetPasswordDto {
    email?: string;
    oldPassword?: string;
    newPassword?: string;
}

export enum UserStatus {
    Unlisted,
    Matched,
    NotMatched
}

export enum UserInterest {
    Unlisted,
    Mammals,
    Reptiles,
    Amphibians,
    Birds
}
export class MatchDetailsDto {
    id?: string;
    user1Name?: string;
    user2Name?: string;
    matchStatus?: number;
    Response1?: number;
    Response2?: number;
}

export class PostDetailsDto {
    postId?: string;
    likesCount?: number;
    likedByUser?: boolean;
    description?: string;
    imageData?: Uint8Array;
}

export enum MatchStatus {
    Success,
    Failure,
    AwaitingResponse
}

export enum UserResponse {
    Accepted,
    Rejected,
    Pending
}