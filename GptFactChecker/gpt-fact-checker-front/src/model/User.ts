class User {
	public id: string;
	public name: string;
	public email: string;
	public userName: string;
    public profileImage: string;
    public password?: string;
    public roles: string[];
    public about: string;

	constructor() {
        this.id = "";
        this.name = "";
        this.email = "";
        this.userName = "";
        this.profileImage = "";
        this.roles = [];
        this.about = "";
	}
}

export default User;
