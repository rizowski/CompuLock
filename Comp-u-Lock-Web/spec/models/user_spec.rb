require 'spec_helper'

describe User do
  it "has a valid factory" do
  	FactoryGirl.create(:user).should be_valid
  end
  it "is invalid without an email" do
  	FactoryGirl.build(:user, email: nil).should_not be_valid
  end
  it "is invalid without a password" do
  	FactoryGirl.build(:user, password: nil).should_not be_valid
  end
  it "does not allow duplicate usernames" do 
  	FactoryGirl.create(:user, username:"Rizowski")
  	FactoryGirl.build(:user, username:"Rziowski").should_not be_valid
  end
  it "does not allow duplicate emails" do 
  	FactoryGirl.create(:user, email: "crouska@gmail.com")
  	FactoryGirl.build(:user, email: "crouska@gmail.com").should_not be_valid
  end
end
