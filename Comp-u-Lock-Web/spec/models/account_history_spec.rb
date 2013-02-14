require 'spec_helper'

describe AccountHistory do
  it "has a valid factory" do
  	FactoryGirl.create(:account_history).should be_valid
  end
end
