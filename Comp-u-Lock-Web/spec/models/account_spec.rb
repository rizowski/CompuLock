require 'spec_helper'

describe Account do
  it "creates an Account and saves it to the db" do
  	user = FactoryGirl.build(:account)
  	assert user.save
  end

  it "creates an Account and a process and saves it to the db" do
  	user = FactoryGirl.build(:account)
  	process = FactoryGirl.build(:account_process)
  	assert user.account_process.find(1)
  end
end
