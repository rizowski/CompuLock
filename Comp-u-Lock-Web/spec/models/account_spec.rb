require 'spec_helper'

describe Account do
	it "creates a valid factory" do
		FactoryGirl.create(:account).should be_valid
	end
  it "is invalid with out a username" do
    FactoryGirl.build(:account, username: nil).should_not be_valid
  end
  it "is invalid with out a computer Id" do 
    FactoryGirl.build(:account, computer_id: nil).should_not be_valid
  end
  it "has an account process once it is added"
  it "has an account history once it is added"
  it "has an account program once it is added"
  
  it "creates an Account and saves it to the db" do
  	account = FactoryGirl.build(:account)
  	assert account.save
  end

  # it "creates an Account and a process and saves it to the db" do
  # 	user = FactoryGirl.build(:account)
  # 	process = FactoryGirl.build(:account_process)
  # 	assert user.account_process.find(1)
  # end
end
