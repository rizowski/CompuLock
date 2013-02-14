require 'spec_helper'

describe Account do
	it "creates a valid factory" do
		FactoryGirl.create(:account).should be_valid
	end
  it "creates an Account and saves it to the db" do
  	user = FactoryGirl.build(:account)
  	assert user.save
  end

  # it "creates an Account and a process and saves it to the db" do
  # 	user = FactoryGirl.build(:account)
  # 	process = FactoryGirl.build(:account_process)
  # 	assert user.account_process.find(1)
  # end
end
