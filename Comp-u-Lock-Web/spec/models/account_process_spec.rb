require 'spec_helper'

describe AccountProcess do
  it "has a valid factory" do
  	FactoryGirl.create(:account_process).should be_valid
  end

end
 