require 'spec_helper'

describe AccountProgram do
  it "has a valid factory" do
  	FactoryGirl.create(:account_program).should be_valid
  end
end
