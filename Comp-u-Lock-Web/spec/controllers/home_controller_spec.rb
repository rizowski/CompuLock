require 'spec_helper'

describe HomeController do
  	describe "Routing" do
	    it "GET does route to #index" do
			{get: "home"}.should route_to(
				controller: "home", 
				action: "index")
		end
  	end
end
