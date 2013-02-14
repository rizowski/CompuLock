require 'spec_helper'

describe Computer do
	it "has a valid factory" do
		FactoryGirl.create(:computer).should be_valid
	end
	it "is invalid without a Name" do 
		FactoryGirl.build(:computer, name: nil).should_not be_valid
	end
	it "is invalid without an Enviroment" do
		FactoryGirl.build(:computer, enviroment: nil).should_not be_valid
	end
	it "is invalid without a user Id" do
		FactoryGirl.build(:computer, user_id: nil).should_not be_valid
	end

    it "successfully creates a computer and saves it to the db" do
  	  	computer = FactoryGirl.build(:computer)
  	  	assert computer.save
    end
end
