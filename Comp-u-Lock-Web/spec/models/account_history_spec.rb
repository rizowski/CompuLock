require 'spec_helper'

describe AccountHistory do
  	it "has a valid factory" do
  		FactoryGirl.create(:account_history).should be_valid
  	end
  	it "is invalid woutout an account Id" do
  		FactoryGirl.build(:account_history, account_id: nil).should_not be_valid
  	end
  	it "is invalid wihtout a domain name" do
  		FactoryGirl.build(:account_history, domain: nil).should_not be_valid
  	end
  	it "is invalid without a last visited date" do
  		FactoryGirl.build(:account_history, last_visited: nil).should_not be_valid
	end
end
