require 'spec_helper'

describe Computer do
  it "creates a user and saves it to the db" do
  	user = FactoryGirl.build(:user)
  	assert user.save
  end
end
